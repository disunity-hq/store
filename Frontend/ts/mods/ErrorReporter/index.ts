import * as ejs from '@vendor/ejs';

import ManifestReport from './reports/ManifestSchemaException.html';
import UnauthorizedReport from './reports/Unauthorized.html';


export default class ErrorReporter {

    private container: JQuery<HTMLElement>;
    private reportMap = new Map<string, ejs.TemplateFunction>([
        ["Unauthorized", ejs.compile(UnauthorizedReport)],
        ["ManifestSchemaException", ejs.compile(ManifestReport)],
    ]);

    constructor(selector: string) {
        this.container = jQuery(selector);
    }

    public Empty() {
        this.container.empty();
    }

    public Set(exception) {
        this.Empty();
        
        var reporter = this.reportMap[exception.type];

        if (reporter) {
            var report = reporter(exception);
            this.container.html(report);
        } else {
            console.log(`Unknown error type: ${exception.type}`);
        }

    }

}
