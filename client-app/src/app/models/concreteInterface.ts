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