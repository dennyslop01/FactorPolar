async function loadPdfFromBase64(base64Data, containerId) {
    try {
        const container = document.getElementById(containerId);
        if (!container) {
            console.warn(`[PDFJS] El contenedor con ID '${containerId}' aún no está listo en el DOM.`);
            return;
        }

        // Limpiar contenido previo para evitar duplicados en re-renders de Blazor
        container.innerHTML = "";

        // Convertir base64 a bytes independientes del navegador
        const binStr = atob(base64Data);
        const len = binStr.length;
        const bytes = new Uint8Array(len);
        for (let i = 0; i < len; i++) {
            bytes[i] = binStr.charCodeAt(i);
        }

        // Cargar el documento PDF
        const pdf = await pdfjsLib.getDocument({ data: bytes }).promise;

        // Recorrer secuencialmente todas las páginas del documento
        for (let pageNum = 1; pageNum <= pdf.numPages; pageNum++) {
            const page = await pdf.getPage(pageNum);

            // Crear dinámicamente un elemento canvas por cada página
            const canvas = document.createElement('canvas');
            canvas.id = `pdf-page-${pageNum}`;
            canvas.style.maxWidth = "100%";
            canvas.style.height = "auto";
            canvas.style.display = "block";
            canvas.style.margin = "0 auto 15px auto"; // Separación estética entre páginas
            canvas.style.boxShadow = "0 2px 5px rgba(0,0,0,0.15)"; // Sombra de documento real

            container.appendChild(canvas);

            const context = canvas.getContext('2d');
            const viewport = page.getViewport({ scale: 1.5 });

            canvas.height = viewport.height;
            canvas.width = viewport.width;

            const renderContext = {
                canvasContext: context,
                viewport: viewport
            };

            // Dibujar la página actual en su respectivo lienzo gráfico
            await page.render(renderContext).promise;
        }
    } catch (e) {
        console.error("Error al renderizar el PDF multipágina:", e);
    }
}


function loadPdfFrameFromBase64(base64Data, frame) {
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
