var FileImporterPlugin = {
  FileImporterCaptureClick: function() {
    if (!document.getElementById('FileImporter')) {
      var fileInput = document.createElement('input');
      fileInput.setAttribute('type', 'file');
      fileInput.setAttribute('id', 'FileImporter');
      fileInput.setAttribute('accept', '.vrm')
      fileInput.style.visibility = 'hidden';
      fileInput.onclick = function (event) {
        this.value = null;
      };
      fileInput.onchange = function (event) {
        SendMessage('VRMLoader', 'FileSelected', URL.createObjectURL(event.target.files[0]));
      }
      document.body.appendChild(fileInput);
    }

    var OpenFileDialog = function() {
      document.getElementById('FileImporter').click();
      document.getElementById('#canvas').removeEventListener('click', OpenFileDialog);
    };

    document.getElementById('#canvas').addEventListener('click', OpenFileDialog, false);
  }
};
mergeInto(LibraryManager.library, FileImporterPlugin);