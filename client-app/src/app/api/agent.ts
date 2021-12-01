import axios, { AxiosResponse } from "axios";
import React from "react";
import { Project } from "../models/coreInterface";
import CommonStore from '../stores/commonStore';
import { store } from "../stores/store";
import { ConcreteParam } from '../models/concreteInterface';

const sleep = (delay: number) => {
    return new Promise((resolve) => {
        setTimeout(resolve, delay);

    })
    
}



axios.defaults.baseURL = 'http://localhost:5000/api';

const responseBody = <T>(response: AxiosResponse<T>) => response.data;

const requests = {
    get: <T>(url: string) => axios.get<T>(url).then(responseBody),
    post: <T>(url: string) => axios.post<T>(url).then(responseBody),
    put: <T>(url: string) => axios.put<T>(url).then(responseBody),
    del: <T>(url: string) => axios.delete<T>(url).then(responseBody),
    download: (url: string) => axios.get(url)
};

const Projects = {
    list: (officeId:number) => requests.get<Project[]>(`/Project/list?officeid=${officeId}`)
}

const FieldConcreteTest = {
    json: (params:ConcreteParam) => requests.download(`/FieldConcrteTest/json?projectId=${params.projectId}&dataset=${params.dataset}&format=${params.format}`),
    excel: (params:ConcreteParam) => requests.download(`/FieldConcrteTest/excel?projectId=${params.projectId}&dataset=${params.dataset}&format=${params.format}`),
    
}

const agent = {
    Projects,
    FieldConcreteTest
}

export default agent;