import * as React from 'react';
import * as ReactDOM from 'react-dom';
import {
    Router, Route, IndexRoute, Link, hashHistory, IndexRedirect,
    RouterState, RedirectFunction
} from 'react-router';
import { Layout } from './layout';
import { Step1View } from './step1';
import { Step2View } from './step2';
import { Step3View } from './step3';
import { onRouteChange } from '../../components/routeStore';
import { addUserStore } from './store';
import 'isomorphic-fetch';
import './styles.scss'
import * as service from './service';

addUserStore.setRoute("/step1");

function routeChange(prevState: RouterState, nextState: RouterState, replace: RedirectFunction, callback: Function) {
    onRouteChange(addUserStore, prevState, nextState, replace, callback);
}

var app = (
    <Router history={hashHistory}>
        <Route path="/" component={Layout} onChange={routeChange}>
            <IndexRedirect to="/step1" />
            <Route path="/step1" component={Step1View} />
            <Route path="/step2" component={Step2View} />
            <Route path="/step3" component={Step3View} />
        </Route>
    </Router>
);

ReactDOM.render(app, document.getElementById("add-user-to-organization-app"));
