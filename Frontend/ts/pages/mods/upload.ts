import { Uploader } from '@syncfusion/ej2-inputs';

import { htmlToElement } from 'utils';

import ErrorReporter from 'shared/ErrorReporter';
import { DropDownList } from '@syncfusion/ej2-dropdowns';

export class ModUploadPage {
  private uploader: Uploader;
  private reporter: ErrorReporter;

  constructor() {
    this.reporter = new ErrorReporter('#error-report');
    this.setupUploader();
  }

  private setupUploader = () => {
    this.uploader = new Uploader({
      multiple: false,
      asyncSettings: { saveUrl: '/api/v1/mods/upload' },
      success: this.uploadSuccess,
      failure: this.uploadFailure,
      uploading: this.reporter.Empty.bind(this.reporter)
    });
    this.uploader.appendTo('#ArchiveUpload');

  };

  private uploadSuccess = (args: any) => {};

  private uploadFailure = (args: any) => {
    var response = JSON.parse(args.e.currentTarget.responseText);
    this.reporter.Set(response);
  };
}

export function InitPageScript() {
  return new ModUploadPage();
}
