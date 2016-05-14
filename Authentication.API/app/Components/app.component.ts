/// <reference path="../../node_modules/angular2/ts/typings/node/node.d.ts"/>
/// <reference path="../../node_modules/angular2/typings/browser.d.ts"/>
import { Component} from 'angular2/core';
import { HTTP_PROVIDERS } from 'angular2/http';
import { LoginComponent} from './login.component';

@Component({
    selector: 'authentication-app',
    templateUrl: 'app/Templates/app.component.html',
    styleUrls: ['app/Styles/app.component.css'],
    directives: [LoginComponent],
    providers: [HTTP_PROVIDERS]
})

export class AppComponent {

}