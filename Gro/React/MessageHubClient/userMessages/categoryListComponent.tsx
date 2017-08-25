import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { Category } from './model/categoryModel';
import { Message } from './model/messageModel';
import { messageSetting } from '../shared/messageSetting';

var $ = window["$"];

interface CategoryRowProps {
    category: Category;

    onClick: () => void;
}

export class CategoryRow extends React.Component<CategoryRowProps, any>{
    constructor(props: CategoryRowProps) {
        super(props);
    }

    handerClick(): void {
        this.props.category.isSelected = !this.props.category.isSelected;
        this.forceUpdate();
        this.props.onClick();
    }

    render() {
        return (
            <li className={this.props.category.isSelected ? "active" : ""}>
                <a href="javascript:void(0);" onClick={_ => this.handerClick()}>{this.props.category.name}</a>
            </li>
        )
    }
}

interface LinkAllProps {
    categories: Category[];
    onClick: (isSelectAll: boolean) => void;
}

export class LinkAll extends React.Component<LinkAllProps, any>{
    private isSelectAll: boolean;

    constructor(props: LinkAllProps) {
        super(props);
        this.isSelectAll = true;
        if (this.props != undefined && this.props.categories != undefined) {
            var result = this.props.categories.find(function (item) {
                return item.isSelected == true;
            }, this);
            this.isSelectAll = result != undefined ? false : true;
        }
    }

    handerClick(): void {
        if (this.isSelectAll) return;
        this.reRender(!this.isSelectAll);
        this.props.onClick(this.isSelectAll);
    }

    reRender(isSelectAll: boolean): void {
        if (this.isSelectAll != isSelectAll) {
            this.forceUpdate();
            this.isSelectAll = isSelectAll;
        }
    }

    render() {
        return (
            <li onClick={_ => this.handerClick()} className={this.isSelectAll ? "active" : ""}>
                <a href="javascript:void(0);" >Alla</a>
            </li>
        )
    }
}

interface CategoryListProps {
    categories: Category[];

    onFilterChange: (selectedCats: Category[]) => void;
}

export class CategoryList extends React.Component<CategoryListProps, any>{

    private linkAll: LinkAll;
    userSettingUrl: string;
    constructor(props: CategoryListProps) {
        super(props);
        this.userSettingUrl = messageSetting.getUserSettingUrl();

    }

    setSelectedCategory(): void {
        var selectedCats = [];
        for (var i = 0; i < this.props.categories.length; i++) {
            if (this.props.categories[i].isSelected) {
                selectedCats.push(this.props.categories[i]);
            }
        }
        if (this.linkAll) this.linkAll.reRender(selectedCats.length == 0 ? true : false);

        this.props.onFilterChange(selectedCats);
    }

    componentWillReceiveProps(newProps): void {
        if (newProps.categories) {
            var result = newProps.categories.find(function (item) {
                return item.isSelected == true;
            }, this);

            if (this.linkAll) this.linkAll.reRender(result != undefined ? false : true);
        }

        //this.isSelectAll= result != undefined ? false : true
    }

    selectAllCategory(): void {
        //var temp = !this.isSelectAll;

        for (var i = 0; i < this.props.categories.length; i++) {
            this.props.categories[i].isSelected = false;
        }
        //this.forceUpdate();
        this.props.onFilterChange([]);
    }

    showFilterOnMobile(): void {
        $(".lm__meddelanden-filters-list .toggle-filter").next().toggle();
    }

    render() {
        var catRows = this.props.categories != null ? this.props.categories.map(function (cat) {
            return (
                <CategoryRow key={cat.id} category={cat} onClick={() => this.setSelectedCategory()} />
            );
        }.bind(this)) : "";

        var linkAll = this.props.categories != null && this.props.categories.length > 0 ?
            <LinkAll onClick={(nv) => this.selectAllCategory()} categories={this.props.categories} ref={r => this.linkAll = r} /> : "";

        return (
            <div className="lm__meddelanden-filters">
                <h3 className="lm__meddelanden-filters-title">Filtrera meddelanden</h3>
                <div className="lm__meddelanden-filters-list">
                    <a href="javascript:void(0);" className="toggle-filter" onClick={_ => this.showFilterOnMobile()}></a>
                    <nav>
                        <ul>
                            {linkAll}
                            {catRows}
                        </ul>
                    </nav>
                </div>
                <a href={this.userSettingUrl} className="lm__meddelanden-setting-btn"><i className="fa fa-cog" aria-hidden="true"></i>Meddelandeinställningar</a>
            </div>
        )
    }
}
