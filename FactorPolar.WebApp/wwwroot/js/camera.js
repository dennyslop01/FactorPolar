let mediaRecorder;
let recordedChunks = [];
let stream;

window.cameraFunctions = {
    startRecording: async (videoElementId, dotnetHelper) => {
        try {
            const videoElement = document.getElementById(videoElementId);

            // CONFIGURACIÓN CLAVE PARA MÓVILES:
            // 1. facingMode: 'user' (Cámara frontal/selfie) o 'environment' (Trasera)
            // 2. width/height: 1280x720 (HD). Balance perfecto calidad/peso.
            stream = await navigator.mediaDevices.getUserMedia({
                video: {
                    facingMode: "user",
                    width: { ideal: 1280 },
                    height: { ideal: 720 }
                },
                audio: true
            });

            videoElement.srcObject = stream;
            videoElement.play();

            // Configurar grabación (intenta usar MP4 o WebM optimizado)
            const options = { mimeType: 'video/webm;codecs=vp9' };
            mediaRecorder = new MediaRecorder(stream, options);

            mediaRecorder.ondataavailable = (event) => {
                if (event.data.size > 0) recordedChunks.push(event.data);
            };

            mediaRecorder.onstop = async () => {
                const blob = new Blob(recordedChunks, { type: 'video/webm' });
                recordedChunks = []; // Limpiar memoria

                // Convertir Blob a ArrayBuffer para enviar a C#
                const arrayBuffer = await blob.arrayBuffer();
                const uint8Array = new Uint8Array(arrayBuffer);

                // Enviar datos a Blazor
                await dotnetHelper.invokeMethodAsync('ProcessVideo', uint8Array);

                // Apagar cámara
                stream.getTracks().forEach(track => track.stop());
            };

            mediaRecorder.start();

            // TEMPORIZADOR DE SEGURIDAD: Detener a los 60 segundos automáticamente
            setTimeout(() => {
                if (mediaRecorder && mediaRecorder.state === "recording") {
                    mediaRecorder.stop();
                    dotnetHelper.invokeMethodAsync('NotifyTimeLimit');
                }
            }, 60000); // 60000 ms = 1 minuto

        } catch (err) {
            console.error("Error accediendo a la cámara:", err);
            alert("No se pudo acceder a la cámara. Verifique los permisos.");
        }
    },

    stopRecording: () => {
        if (mediaRecorder && mediaRecorder.state === "recording") {
            mediaRecorder.stop();
        }
    }
};