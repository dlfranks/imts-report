
import { makeAutoObservable, runInAction } from "mobx";
import React from "react";
import agent from "../api/agent";
import { ConcreteParam, ConcreteTableSamples } from '../models/concreteInterface';
import FileSaver from 'file-saver';
import { ObjectSchema } from "yup";

export interface TableDataset{
    data: LooseObject[];
    
}
export interface LooseObject {
    [key: string]: any
}

export default class ConcreteStore {
    samples : ConcreteTableSamples = { full: [], strength: [], mixNumber: [] };;
    loadingInitial = false;
    downloaded = false;
    sampleReady = false;

    constructor() {
        makeAutoObservable(this)
    }

    concreteApiUrl = 'http://localhost:5000/api/FieldConcreteTest';
    downloadExcel = async (params: ConcreteParam) => {
        this.downloaded = true;
        if (this.downloaded) {
            const link = document.createElement("a");
            link.href = `http://localhost:5000/api/FieldConcreteTest/excel?projectId=${params.projectId}&dataset=${params.dataset}`;
            link.click();
            this.downloaded = false;
        }
    }

    downloadJson = async (params: ConcreteParam) => {
        this.downloaded = true;
        if (this.downloaded) {
            const link = document.createElement("a");
            link.href = `http://localhost:5000/api/FieldConcreteTest/downloadJson?projectId=${params.projectId}&dataset=${params.dataset}`;
            link.click();
            this.downloaded = false;
        }
    }

    downloadXml = async (params: ConcreteParam) => {
        this.downloaded = true;
        if (this.downloaded) {
            const link = document.createElement("a");
            link.href = `http://localhost:5000/api/FieldConcreteTest/downloadXml?projectId=${params.projectId}&dataset=${params.dataset}`;
            link.click();
            this.downloaded = false;
        }
    }

    getSamples = async () => {
        try {
            await agent.Concrete.samples().then((data) => {
                
                
                const { Table1, Table2, Table3 } = data;
                
                const samples: ConcreteTableSamples = { full: [], strength: [], mixNumber: [] };

                let tableKeys = (Object.keys(data) as Array<keyof typeof data>).reduce((accumulator, current) => {
                    accumulator.push(current);
                    return accumulator;
                }, [] as (typeof data[keyof typeof data])[]);

                
                
                tableKeys.forEach(tableKey => {
                    
                    if (tableKey === 'Table1') {
                        let table: LooseObject[] = data[tableKey] as LooseObject[];
                        samples.full = table;
                        console.log(table[0]);
                    } else if (tableKey === 'Table2') {
                        let table: LooseObject[] = data[tableKey] as LooseObject[];
                        samples["strength"] = table;
                    } else if(tableKey === 'Table3'){
                        let table: LooseObject[] = data[tableKey] as LooseObject[];
                        samples["mixNumber"] = table;
                    }
                    

                });
                runInAction(() => {
                    this.samples = { ...samples };
                    this.sampleReady = true;
                });
                
            });
            
            
        }
        catch (error) {
            this.sampleReady = false;
        }
    }
    setSampleReady = (state:boolean) => {
        this.sampleReady = state;
    }

}