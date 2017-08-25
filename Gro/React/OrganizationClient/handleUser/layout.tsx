import * as React from 'react';
import * as ReactDOM from 'react-dom';

export class Layout extends React.Component<{}, any>{

    render() {
        return (
            <div className="lm__add-user__wrapper" >
                {this.props.children}
            </div>
        );
    }
}
