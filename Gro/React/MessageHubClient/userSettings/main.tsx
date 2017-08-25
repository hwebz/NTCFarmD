import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { Router, Route, IndexRoute, Link, browserHistory } from 'react-router';
import { UserSettingPage } from './userSettingsPage';
import 'isomorphic-fetch';

ReactDOM.render(<UserSettingPage />, document.getElementById("user-message-settings-app"));
