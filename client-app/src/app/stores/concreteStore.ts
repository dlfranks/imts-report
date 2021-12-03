import { makeAutoObservable } from "mobx";
import React from "react";
import agent from "../api/agent";
import { ConcreteParam } from '../models/concreteInterface';
import FileSaver from 'file-saver';

export default class ConcreteStore {
    loadingInitial = false;
    downloaded = false;

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

}