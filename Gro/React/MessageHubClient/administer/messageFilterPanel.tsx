import * as React from 'react';
import { MessageCategory, MessageType, MessageItem } from './administerModels';
import * as adminService from './services';
import { messageStore } from './messageStore';

interface MessageFilterPanelState {
    categories: MessageCategory[];
    selectedCate?: number;
    selectedType?: number;
}

const timeoutInterval = 150;

export class MessageFilterPanel extends React.Component<any, MessageFilterPanelState>{
    private actionTimeout: number;

    constructor() {
        super();
        this.state = {
            categories: [],
            selectedCate: 0,
            selectedType: 0
        };
    }

    selectAction = (categoryIds: number[], typeIds: number[]) => {
        adminService.changeFilter(categoryIds, typeIds);
    };

    componentDidMount() {

        //load everything first
        adminService.getAllCategories(true).then(cts => {
            this.setState({
                categories: cts
            });
        }).catch(reason => {
            console.log(`cannot fetch categories ${reason}`)
        });
    }

    categoryClicked(cate: MessageCategory) {
        //ignore if the same filter is selected
        if (!cate && this.state.selectedCate == 0 && this.state.selectedType == 0) {
            return;
        }

        if (!!cate && cate.CategoryId == this.state.selectedCate && this.state.selectedType == 0) {
            return;
        }

        if (!cate) {
            clearTimeout(this.actionTimeout);
            this.actionTimeout = window.setTimeout(() => this.selectAction([], []), timeoutInterval);

            this.setState({
                categories: this.state.categories,
                selectedCate: 0,
                selectedType: 0
            });
            return;
        }

        clearTimeout(this.actionTimeout);
        this.actionTimeout = window.setTimeout(() => this.selectAction([cate.CategoryId], []), timeoutInterval);

        this.setState({
            categories: this.state.categories,
            selectedCate: cate.CategoryId,
            selectedType: 0
        });
    }

    messageTypeClicked(type: MessageType) {
        //ignore if the same filter is selected
        if (type.CategoryId == this.state.selectedCate && this.state.selectedType == type.TypeId) {
            return;
        }

        clearTimeout(this.actionTimeout);
        this.actionTimeout = window.setTimeout(() => this.selectAction([type.CategoryId], [type.TypeId]), timeoutInterval);

        this.setState({
            categories: this.state.categories,
            selectedCate: type.CategoryId,
            selectedType: type.TypeId
        });
    }

    render() {

        return (
            <nav className="meddelande-main-nav">
                <ul>
                    <li className={this.state.selectedCate <= 0 && this.state.selectedType <= 0 ? 'active' : ''} >
                        <a href="javascript:void(0)" onClick={_ => this.categoryClicked(null)}> Alla meddelanden </a>
                    </li>
                    {this.state.categories.map((cate, index) => {
                        let activeCateClass = this.state.selectedCate == cate.CategoryId ? 'active' : '';
                        return (
                            <li key={index} className={activeCateClass} >
                                <a href="javascript:void(0)" onClick={_ => this.categoryClicked(cate)} > {cate.CategoryName}</a>
                                <ul className="meddelande-sub-nav">
                                    {cate.Types.map((type, idxCate) => {
                                        let activeTypeClass = this.state.selectedType == type.TypeId && this.state.selectedCate == type.CategoryId ?
                                            'active' : '';
                                        return (
                                            <li key={type.TypeId} className={activeTypeClass}>
                                                <a href="javascript:void(0)" onClick={e => this.messageTypeClicked(type)}  >{type.TypeName}</a>
                                            </li>
                                        );
                                    })}
                                </ul>
                            </li>
                        );
                    })}
                </ul>
            </nav>
        );
    }
}
