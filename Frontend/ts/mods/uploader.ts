import { Uploader } from '@syncfusion/ej2-inputs'

import { htmlToElement } from 'utils';

import ErrorReporter from './ErrorReporter';


export class ModUploadPage {

    private uploader: Uploader;
    private reporter: ErrorReporter;

    constructor() {
        this.setupUploader();
        this.reporter = new ErrorReporter('#error-report');
    }

    private setupUploader() {
        var _this = this;
        this.uploader = new Uploader({
            multiple: false,
            asyncSettings: { saveUrl: '/api/v1/mods/upload' },
            success: this.uploadSuccess,
            failure: e => _this.uploadFailure(e),
            uploading: e => _this.reporter.Empty(),
        });
        this.uploader.appendTo('#ArchiveUpload');
    }

    private uploadSuccess(args: any) { }


    private uploadFailure(args: any) {
        var response = JSON.parse(args.e.currentTarget.responseText);
        this.reporter.Set(response);
    }
}

export function SetupModUploadPage() {
    return new ModUploadPage();
}