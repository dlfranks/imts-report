import axios, { AxiosError, AxiosResponse } from "axios";
import { Project } from "../models/coreInterface";
import { store } from "../stores/store";
import { ConcreteParam } from '../models/concreteInterface';
import { User, UserFormValues } from "../models/user";
import { toast } from "react-toastify";
import { history } from '../../index';


const sleep = (delay: number) => {
    return new Promise((resolve) => {
        setTimeout(resolve, delay);

    })
    
}

axios.interceptors.request.use(config => {
    const token = store.commonStore.token;
    if (config.headers === undefined) config.headers = {};
    if (token) config.headers.Authorization = `Bearer ${token}`;
    return config;
})

axios.interceptors.response.use(async response => {
    await sleep(1000);
    return response;
}, (error: AxiosError) => {
    const { data, status, config } = error.response!;
    console.log(error.response);
    switch (status) {
        case 400:
            if (typeof data === 'string') {
                toast.error(data);
            }
            if (config.method === 'get' && data.errors.hasOwnProperty('id')) {
                history.push('/not-found');
            }
            if (data.errors) {
                const modalStateErrors = [];
                for (const key in data.errors) {
                    if (data.errors[key]) {
                        modalStateErrors.push(data.errors[key]);
                    }
                }
                throw modalStateErrors.flat();
            } else {
                toast.error(data);
            }
            
            break;
        case 401:
            toast.error('unauthorised');
            break;
        case 404:
            history.push('/not-found');
            break;
        case 500:
            store.commonStore.setServerError(data);
            history.push('/server-error');
            break;
    }
    return Promise.reject(error);
});


axios.defaults.baseURL = 'http://localhost:5000/api';

const responseBody = <T>(response: AxiosResponse<T>) => response.data;

const requests = {
    get: <T>(url: string) => axios.get<T>(url).then(responseBody),
    post: <T>(url: string, body:{}) => axios.post<T>(url, body).then(responseBody),
    put: <T>(url: string, body:{}) => axios.put<T>(url, body).then(responseBody),
    del: <T>(url: string) => axios.delete<T>(url).then(responseBody),
    
};

const Projects = {
    list: (officeId:number) => requests.get<Project[]>(`/Project/list?officeid=${officeId}`)
}

const Concrete = {
    json: (params: ConcreteParam) => {
        axios.get(`/FieldConcreteTest/json?projectId=${params.projectId}&dataset=${params.dataset}&format=${params.format}`);
    },
    samples: () => {
        //requests.get<ConcreteTable[]>(`/FieldConcrteTest/samples`)
        const url = `/FieldConcreteTest/samples`;
        return axios.get(url).then((responseBody));
    },
    download: (url:string) => {
        axios.get(url);
    }
}

const Account = {
    current: () => requests.get<User>('/account'),
    login: (user: UserFormValues) => requests.post<User>('/account/login', user),
    register: (user: UserFormValues) => requests.post<User>('/account/register', user)
}

const agent = {
    Projects,
    Concrete, 
    Account
}

export default agent;