import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { Router, Route, IndexRoute, Link, hashHistory } from 'react-router';
import { Layout } from './layout';
import { AllUsersView } from './allUsers';
import { SpecificUserView } from './specificUser';
import 'isomorphic-fetch';
import './styles.scss'

var app = (
    <Router history={hashHistory}>
        <Route component={Layout}>
            <Route path="/" component={AllUsersView} />
            <Route path="/specific" component={SpecificUserView} />
        </Route>
    </Router>
);

ReactDOM.render(app, document.getElementById("handle-organization-user-app"));
