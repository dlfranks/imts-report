import { makeAutoObservable, runInAction } from "mobx";
import React from "react";
import agent from "../api/agent";
import { ConcreteParam } from "../models/concreteInterface";
import fileSaver from "file-saver";

export interface TableDataset {
  data: LooseObject[];
}
export interface LooseObject {
  [key: string]: any;
}

export default class ConcreteStore {
  samples = { full: [], strength: [], mixNumber: [] };
  loadingInitial = false;
  downloaded = false;
  sampleReady = false;

  constructor() {
    makeAutoObservable(this);
  }

  concreteApiUrl = "http://localhost:5000/api/FieldConcreteTest";

  setLoadingInitial = (state: boolean) => {
    this.loadingInitial = state;
  };

  getJson = async (param: ConcreteParam) => {
    this.setLoadingInitial(true);
    let data = await agent.Concrete.json(param);
    //let file = new Blob([data], {type: 'application/pdf'});
    //let fileURL = URL.createObjectURL(file);
    //let w = window.open(fileURL);
    let w = window.open("http://localhost:3000/json");
    w?.document.write(JSON.stringify(data));
    this.setLoadingInitial(false);
  };

  downloadExcel = async (params: ConcreteParam) => {
    this.downloaded = true;
    let url = `${this.concreteApiUrl}/excel?projectId=${params.projectId}&dataset=${params.dataset}`;
    const result = await agent.Concrete.excel(url);

    runInAction(() => {
      this.downloaded = false;
    });

    const url2 = window.URL.createObjectURL(new Blob([result]));
    const link = document.createElement("a");
    link.href = url2;
    link.setAttribute("download", "file.xlsx");
    link.click();

    // if (this.downloaded) {
    //   const link = document.createElement("a");
    //   link.href = `http://localhost:5000/api/FieldConcreteTest/excel?projectId=${params.projectId}&dataset=${params.dataset}`;
    //   link.click();
    //   this.downloaded = false;
    // }
  };

  downloadJson = async (params: ConcreteParam) => {
    this.downloaded = true;
    if (this.downloaded) {
      const link = document.createElement("a");
      link.href = `http://localhost:5000/api/FieldConcreteTest/downloadJson?projectId=${params.projectId}&dataset=${params.dataset}`;
      link.click();
      this.downloaded = false;
    }
  };

  downloadXml = async (params: ConcreteParam) => {
    this.downloaded = true;
    if (this.downloaded) {
      const link = document.createElement("a");
      link.href = `http://localhost:5000/api/FieldConcreteTest/downloadXml?projectId=${params.projectId}&dataset=${params.dataset}`;
      link.click();
      this.downloaded = false;
    }
  };

  getSamples = async () => {
    try {
      await agent.Concrete.samples().then((data) => {
        const { Table1, Table2, Table3 } = data;

        const samples = { full: [], strength: [], mixNumber: [] };

        let tableKeys = (Object.keys(data) as Array<keyof typeof data>).reduce(
          (accumulator, current) => {
            accumulator.push(current);
            return accumulator;
          },
          [] as typeof data[keyof typeof data][]
        );

        tableKeys.forEach((tableKey) => {
          if (tableKey === "Table1") {
            let table = data[tableKey];
            samples.full = table;
            console.log(table[0]);
          } else if (tableKey === "Table2") {
            let table = data[tableKey];
            samples["strength"] = table;
          } else if (tableKey === "Table3") {
            let table = data[tableKey];
            samples["mixNumber"] = table;
          }
        });
        runInAction(() => {
          this.samples = { ...samples };
          this.sampleReady = true;
        });
      });
    } catch (error) {
      this.sampleReady = false;
    }
  };

  setSampleReady = (state: boolean) => {
    this.sampleReady = state;
  };
}
