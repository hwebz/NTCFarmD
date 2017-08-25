import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { Link, hashHistory } from 'react-router';
import { addUserStore } from './store';
import { BankIdLogin } from '../shared/_bankIdLogin';
import { StepViewer } from '../../components/stepViewer';
import * as service from './service';

export class Layout extends React.Component<{}, any>{

    render() {
        let step1DoneClass = addUserStore.isStep1Done() ? "done" : "";
        let step2DoneClass = addUserStore.step2Done ? "done" : "";
        let activeclassName = !window["serialNumber"] ? "inactive-area" : "";
        return (
            <div className="lm__add-user__wrapper">
                <div className="lm__block-box">
                    <h1 className="lm__page-title no-margin">Lägg till användare</h1>
                </div>
                {!window["serialNumber"] ? <BankIdLogin /> : null}
                <div className={activeclassName}>
                    <StepViewer routeStore={addUserStore} steps={[
                        {
                            index: 1,
                            iconClass: addUserStore.isStep1Done() ? 'done' : '',
                            description: "Personlig information",
                            route: "/step1"
                        },
                        {
                            index: 2,
                            iconClass: addUserStore.step2Done ? 'done' : '',
                            description: "Profil och behörighet",
                            route: "/step2"
                        },
                        {
                            index: 3,
                            iconClass: "",
                            description: "Granska och skicka",
                            route: "/step3"
                        }
                    ]} />
                    {this.props.children}
                </div>
            </div>
        );
    }
}
