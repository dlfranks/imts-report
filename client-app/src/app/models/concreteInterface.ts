import { LooseObject } from './../stores/concreteStore';
import React from "react";
import internal from "stream";

export interface ConcreteParam {
    projectId: number;
    dataset: number;
    format: string;
}



export interface ConcreteTableSamples {
    full: LooseObject[];
    strength: LooseObject[];
    mixNumber: LooseObject[];
    
}

interface ConcreteDatasetOptionModel{
    text: string,
    value:number
}
export const ConcreteDatasetOptions = [
    { text: 'Full', value: 1 },
    { text: 'Strength', value: 2 },
    {text: 'Mix Number', value: 3}
]