window.uploadFunctions = {
    uploadFile: (inputId, dotnetHelper, folderId) => {
        const input = document.getElementById(inputId);
        const file = input.files[0];

        const formData = new FormData();
        formData.append("file", file);
        formData.append("folderId", folderId);

        const xhr = new XMLHttpRequest();
        xhr.open("POST", "/api/Upload", true);

        // ESTA ES LA CLAVE: El navegador reporta el progreso nativamente
        xhr.upload.onprogress = (e) => {
            if (e.lengthComputable) {
                const percent = Math.round((e.loaded / e.total) * 100);
                // Le avisamos a Blazor solo para pintar la barrita
                dotnetHelper.invokeMethodAsync('UpdateProgress', percent);
            }
        };

        xhr.onload = () => {
            if (xhr.status === 200) {
                const response = JSON.parse(xhr.responseText);
                dotnetHelper.invokeMethodAsync('UploadComplete', response.id);
            } else {
                dotnetHelper.invokeMethodAsync('UploadFailed', "Error servidor: " + xhr.status);
            }
        };

        xhr.onerror = () => {
            dotnetHelper.invokeMethodAsync('UploadFailed', "Error de red crítico");
        };

        xhr.send(formData);
    }
};