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

const Concrete = {
    json: (params: ConcreteParam) => {
        requests.download(`/FieldConcreteTest/json?projectId=${params.projectId}&dataset=${params.dataset}&format=${params.format}`);
    },
    samples: () => {
        //requests.get<ConcreteTable[]>(`/FieldConcrteTest/samples`)
        const url = `/FieldConcreteTest/samples`;
        return axios.get(url).then((responseBody));
    }
    
    
    
}

const agent = {
    Projects,
    Concrete
}

export default agent;