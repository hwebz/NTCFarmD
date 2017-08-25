import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { scrollToTop, calculateBreadcrumStep } from './dom';
import { Link, hashHistory } from 'react-router';
import { RouteStore } from './routeStore';

interface Step {
    route: string;
    index: number;
    iconClass: string;
    description: string;
}

interface StepViewerProps {
    routeStore: RouteStore;
    steps: Step[];
}

export class StepViewer extends React.Component<StepViewerProps, {}>{
    constructor(props) {
        super(props);
    }

    onRouteChange = () => {
        this.forceUpdate();
        scrollToTop();
    };

    componentDidMount() {
        calculateBreadcrumStep();
        this.props.routeStore.addListener("route", this.onRouteChange);
    }

    componentWillUnmount() {
        this.props.routeStore.removeListener("route", this.onRouteChange);
    }

    componentDidUpdate() {
        calculateBreadcrumStep();
    }

    getStepIconClass(currentStep: Step) {
        if (!currentStep) { return ""; }
        if (this.props.routeStore.route === currentStep.route) { return "active"; }
        return currentStep.iconClass;
    }

    render() {
        var currentStepFilter = this.props.steps.filter(s => s.route === this.props.routeStore.route);
        var currentStep = currentStepFilter.length > 0 ? currentStepFilter[0] : null;
        var currentIndex = !currentStep ? 0 : currentStepFilter[0].index;
        var currentDescription = !currentStep ? "" : currentStepFilter[0].description;

        return (
            <div className="lm__breadcrums">
                <div className="lm__step-inform">
                    <h2 className="heading-title">Steg {currentIndex}</h2>
                    <h3 className="heading-title-2">{currentDescription}</h3>
                </div>

                <ul className="lm__form-progress">
                    <span className="lm__form-progress__line"></span>
                    {this.props.steps.sort((s1, s2) => s1.index - s2.index).map(s =>
                        <li key={s.index} className={this.getStepIconClass(s)}><Link to={s.route}>{s.index}</Link></li>
                    )}
                </ul>
            </div>
        );
    }
}
