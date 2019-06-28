import { UploadingEventArgs } from '@syncfusion/ej2-inputs'


export function OnModUpload(args: UploadingEventArgs) {
    var elements = document.getElementsByName('__RequestVerificationToken');
    var element = elements[0] as HTMLInputElement;
    args.currentRequest.setRequestHeader('XSRF-TOKEN', element.value);
}