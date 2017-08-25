import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { Router, Route, IndexRoute, Link, hashHistory, Redirect } from 'react-router';
import { UserMessagesPage } from './userMessagesPage';
import { MessageTabContent } from './messageTabComponent';
import { MessageDetail } from './messageDetailComponent';

const NotFound = () => <h1>Not Found!</h1>

var App = (
    <Router history={hashHistory}>
        <Redirect from="/" to="messages/inbox" />
        <Route path="/" component={UserMessagesPage}>

            <Redirect from="messages" to="messages/inbox" />
            <Route path="messages/:tabid" component={MessageTabContent}>
                <Route path=":mesid" component={MessageDetail} />
            </Route>
            <Route path='*' component={NotFound} />
        </Route>
    </Router>
);
// var App = ( <IndexRoute component = {(props)=> <CustomRedirect to="/messages/inbox"/>} />
//     <Router history={hashHistory}>
//         <Route path="/" component={(props)=> <MessageTab  children={props.children} />}>
//             <IndexRoute component = {MessageTabContent} />
//             <Redirect from="" to="messages/inbox" />     
//             <Redirect from="messages" to="messages/inbox" />     
//             <Route path="messages/:tabid" component={MessageTabContent}></Route>
//             <Route path="messages/:tabid/:mesid" component={MessageDetail}></Route>
//             <Route path='*' component={NotFound} />
//         </Route>

//     </Router>
// );



//ReactDOM.render(<MessageTab inboxInfo= />, document.getElementById("user-message-app")) <IndexRoute component = {MessageTabContent} />
ReactDOM.render(App, document.getElementById("user-message-app"))

