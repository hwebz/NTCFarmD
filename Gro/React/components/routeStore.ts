import { EventEmitter } from 'events';
import {
    Router, Route, IndexRoute, Link, hashHistory, IndexRedirect,
    RouterState, RedirectFunction
} from 'react-router';

export abstract class RouteStore extends EventEmitter {
    public route = "/";

    public abstract canNavigate(route: string): boolean;

    setRoute(route: string) {
        if (!this.canNavigate(route)) {
        }

        this.route = route;
        this.emit("route");
    }
}

export function onRouteChange(store: RouteStore, prevState: RouterState, nextState: RouterState, replace: RedirectFunction, callback: Function) {
    if (store.canNavigate(nextState.location.pathname)) {
        store.setRoute(nextState.location.pathname);
        callback();
    } else {
        hashHistory.goBack();
    }
}
