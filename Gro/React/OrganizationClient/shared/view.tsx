import * as React from 'react';

export class View<TProps, TState> extends React.Component<TProps, TState>{
    componentDidMount() {
        try {
            window.scrollTo(0, 0);
        } catch (e) {

        }

        try {
            document.body.scrollTop = 0;
        } catch (e) {

        }
    }
}
