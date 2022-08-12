import { dataViewObjectsParser } from "powerbi-visuals-utils-dataviewutils";
import DataViewObjectsParser = dataViewObjectsParser.DataViewObjectsParser;
export declare class ModelSettings {
    url: string;
}
export declare class VisualSettings extends DataViewObjectsParser {
    model: ModelSettings;
}
