import { SettingRow, SettingCategory, SettingTab } from './settingModels';
import { EventEmitter } from 'events';
import * as Immutable from 'immutable';

//Map tab: DisplayOrder -> SettingTab
//Map category : id -> category
//List : [index] => MessageType (item)

export class SettingStore extends EventEmitter {
    private store = Immutable.Map<string, any>();
    initialSettingItems: Immutable.List<SettingRow>;

    constructor() {
        super();
        this.store = this.store
            .set("tabs", Immutable.Map<number, SettingTab>())
            .set("categories", Immutable.Map<number, SettingCategory>())
            .set("rows", Immutable.List<SettingRow>());
    }

    setInitialSetting(settingTabs: SettingTab[], categories: SettingCategory[], settingRows: SettingRow[]) {
        var tabs = this.store.get("tabs") as Immutable.Map<number, SettingTab>;
        tabs = tabs.clear();
        for (var tab of settingTabs) {
            tabs = tabs.set(tab.displayOrder, tab);
        }

        var cates = this.store.get("categories") as Immutable.Map<number, SettingCategory>;
        cates = cates.clear();
        for (var category of categories) {
            cates = cates.set(category.categoryId, category);
        }

        var rows = this.store.get("rows") as Immutable.List<SettingRow>;
        rows = rows.clear();
        for (var row of settingRows) {
            rows = rows.push(row);
        }

        this.store = this.store
            .set("tabs", tabs)
            .set("categories", cates)
            .set("rows", rows);

        this.initialSettingItems = rows;
        this.emit("change");
    }

    tabs(): Immutable.Map<number, SettingTab> {
        return this.store.get("tabs") as Immutable.Map<number, SettingTab>;
    }

    categories(): Immutable.Map<number, SettingCategory> {
        return this.store.get("categories") as Immutable.Map<number, SettingCategory>;
    }

    rows(): Immutable.List<SettingRow> {
        return this.store.get("rows") as Immutable.List<SettingRow>;
    }

    updateRow(id: number, data: SettingRow, categoryId: number) {
        var row = this.rows().filter(r => r.id == data.id && r.categoryId == categoryId).get(0);
        if (!row) {
            throw `Row ${id} not found`;
        }

        var index = this.rows().indexOf(row);
        var newRows = this.rows().set(index, data);
        this.store = this.store.set("rows", newRows);
        this.emit("change");
    }

    saveSuccess() {
        this.emit("saveSuccess");
    }

    resetSettings() {
        this.store = this.store.set("rows", this.initialSettingItems);
        this.emit("change");
    }
}

export var store = new SettingStore();
