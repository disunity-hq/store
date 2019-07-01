import * as ejs from '@vendor/ejs';

import ErrorReport from './error-report.html';


export default class ErrorReporter {

    private container: JQuery<HTMLElement>;
    private template: ejs.TemplateFunction;

    constructor(selector: string) {
        this.container = jQuery(selector);
        this.template = ejs.compile(ErrorReport);
    }

    public Empty() {
        this.container.empty();
    }

    public Set(errors) {
        let report = this.template({
            title: "Manifest errors:",
            errors: errors,
        });

        this.container.html(report);
    }

}
