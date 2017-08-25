import * as React from 'react';
import * as ReactDOM from 'react-dom';

import "./styles.scss";

export interface SpinnerProps {
    color: string;
    size: number;
    thickness: number;
    period: number;
    className?: string;
}

export class Spinner extends React.Component<SpinnerProps, any>{
    render() {
        let styles: React.CSSProperties = {
            border: `${this.props.thickness}px solid transparent`,
            borderRadius: "50%",
            borderTop: `${this.props.thickness}px solid ${this.props.color}`,
            borderLeft: `${this.props.thickness}px solid ${this.props.color}`,
            borderRight: `${this.props.thickness}px solid ${this.props.color}`,
            width: `${this.props.size}px`,
            height: `${this.props.size}px`,
            animation: `spin ${this.props.period}s linear infinite`,
            marginLeft: "auto",
            marginRight: "auto"
        };

        return (
            <div className={"spinner-loader " + this.props.className} style={styles}></div>
        );
    }
}
