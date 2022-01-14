
import { makeAutoObservable } from "mobx";
import React from "react";
import agent from "../api/agent";
import { ConcreteParam } from '../models/concreteInterface';
import FileSaver from 'file-saver';

export interface ConcreteTable{
    [key:string]: LooseObject[],
    
}
export interface LooseObject {
    [key: string]: any
}

export default class ConcreteStore {
    samples: ConcreteTable[] | undefined = undefined;
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
            const data = await agent.Concrete.samples();
            // this.sampleReady = true;
            
        }
        catch (error) {
        
        }
    }
    setSampleReady = (state:boolean) => {
        this.sampleReady = state;
    }

}