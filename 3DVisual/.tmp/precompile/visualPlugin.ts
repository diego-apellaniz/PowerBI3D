import { CustomVisual } from "../../src/CustomVisual";
import powerbiVisualsApi from "powerbi-visuals-api";
import IVisualPlugin = powerbiVisualsApi.visuals.plugins.IVisualPlugin;
import VisualConstructorOptions = powerbiVisualsApi.extensibility.visual.VisualConstructorOptions;
import DialogConstructorOptions = powerbiVisualsApi.extensibility.visual.DialogConstructorOptions;
var powerbiKey: any = "powerbi";
var powerbi: any = window[powerbiKey];
var circleCard2955BB329D6640318943EF420537E619: IVisualPlugin = {
    name: 'circleCard2955BB329D6640318943EF420537E619',
    displayName: '3D Visor',
    class: 'CustomVisual',
    apiVersion: '3.8.0',
    create: (options: VisualConstructorOptions) => {
        if (CustomVisual) {
            return new CustomVisual(options);
        }
        throw 'Visual instance not found';
    },
    createModalDialog: (dialogId: string, options: DialogConstructorOptions, initialState: object) => {
        const dialogRegistry = globalThis.dialogRegistry;
        if (dialogId in dialogRegistry) {
            new dialogRegistry[dialogId](options, initialState);
        }
    },
    custom: true
};
if (typeof powerbi !== "undefined") {
    powerbi.visuals = powerbi.visuals || {};
    powerbi.visuals.plugins = powerbi.visuals.plugins || {};
    powerbi.visuals.plugins["circleCard2955BB329D6640318943EF420537E619"] = circleCard2955BB329D6640318943EF420537E619;
}
export default circleCard2955BB329D6640318943EF420537E619;