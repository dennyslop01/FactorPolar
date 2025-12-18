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
                recordedChunks = [];

                // 1. Guardamos el blob en una variable global temporal
                window.currentVideoBlob = blob;

                // 2. Solo avisamos a C# que ya terminamos (sin enviar los datos todavía)
                await dotnetHelper.invokeMethodAsync('NotifyVideoReady');

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
            stopRecording();
        }
        console.log("startRecording");
    },

    stopRecording: () => {
        console.log("stopRecording");
        if (mediaRecorder && mediaRecorder.state === "recording") {
            mediaRecorder.stop();
        }
    },

    getVideoStream: () => {
        if (window.currentVideoBlob) {
            // Blazor .NET 9 prefiere recibir el Blob directamente
            // y él se encarga de crear el stream reference internamente.
            return window.currentVideoBlob;
        }
        return null;
    }
};