import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { Router, Route, IndexRoute, Link, hashHistory } from 'react-router';
import { AdministerPage } from './administerPage';
import { FreeMessageForm } from './createMessage/freeMessageForm';
import { StandardMessageForm } from './createMessage/standardMessageForm';
import { AdministerMessageList } from './administerMessageList'
import { MessageDetailPage } from './messageDetailPage';
import { CreateMessageLayout } from './createMessage/layout';
import 'isomorphic-fetch';
import './styles.scss';

//import "../../node_modules/trumbowyg/dist/ui/trumbowyg.css";

var AdminApp = (
    <Router history={hashHistory}>
        <Route path="/" component={AdministerPage}>
            <IndexRoute component={AdministerMessageList} />
            <Route path="/message-detail/:messageId" component={MessageDetailPage} />
        </Route>
        <Route path="/create-message" component={CreateMessageLayout} >
            <Route path="/create-message/free" component={FreeMessageForm} />
            <Route path="/create-message/standard" component={StandardMessageForm} />
        </Route>
    </Router>
);

ReactDOM.render(AdminApp, document.getElementById("administer-message-app"))
