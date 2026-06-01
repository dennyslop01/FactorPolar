function loadPdfFromBase64(base64Data, frame) {
    // Función para convertir base64 a Blob
    const base64ToBlob = (base64, type = "application/pdf") => {
        const binStr = atob(base64);
        const len = binStr.length;
        const arr = new Uint8Array(len);
        for (let i = 0; i < len; i++) {
            arr[i] = binStr.charCodeAt(i);
        }
        return new Blob([arr], { type: type });
    };

    // Crear el Blob a partir de los datos Base64
    const blob = base64ToBlob(base64Data, 'application/pdf');

    // Crear una URL de objeto (Blob URL)
    const url = URL.createObjectURL(blob);

    // Asignar la URL al src del iframe
    const iframe = document.getElementById(frame);
    if (iframe) {
        iframe.src = url + '#toolbar=0';
    }
}
