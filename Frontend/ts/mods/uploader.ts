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

    private getListingForFile(filename: string) {
        return jQuery(`li[data-file-name='${filename}']`);
    }

    private getErrorsForFile(filename: string) {
        var listing = this.getListingForFile(filename);
        return jQuery("#errors");
    }

    private clearErrors(filename: string) {
        var errors = this.getErrorsForFile(filename);
        console.log("Errors:");
        console.log(errors);
        errors.remove();
    }

    private setManifestSchemaErrors(filename: string, errors: any[]) {
        let errorItems: string[] = errors.map(e => `<li>${e.message}</li>`);
        let errorListing = `<div><h2>Schema errors:</h2><ul>${errorItems.join()}</ul></div>`;
        console.log(errorListing);
        let errorListingElement = htmlToElement(errorListing);
        let listingForFile = this.getErrorsForFile(filename);
        listingForFile.append(errorListingElement as HTMLElement);
    }

    private setErrors(filename: string, error: any) {
        switch (error.type) {
            case "ManifestSchemaException": {
                this.reporter.Set(error.errors);
                break;
            }
        }
    }

    private uploadFailure(args: any) {
        var filename = args.file.name;
        var response = JSON.parse(args.e.currentTarget.responseText);
        this.reporter.Empty();
        this.setErrors(filename, response);
    }
}

export function SetupModUploadPage() {
    return new ModUploadPage();
}