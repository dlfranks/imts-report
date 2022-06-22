import axios, { AxiosError, AxiosResponse } from "axios";
import { Project } from "../models/coreInterface";
import { store } from "../stores/store";
import { ConcreteParam } from '../models/concreteInterface';
import { User, UserFormValues, LookupUser } from '../models/user';
import { toast } from "react-toastify";
import { history } from '../../index';
import { request } from "http";
import { IAppUser } from '../models/user';


const sleep = (delay: number) => {
    return new Promise((resolve) => {
        setTimeout(resolve, delay);

    })
    
}
axios.defaults.baseURL = 'http://localhost:5000/api';

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
    current: () => requests.get<User>('/account/getcurrentuser'),
    login: (user: UserFormValues) => requests.post<User>('/account/login', user),
    register: (user: UserFormValues) => requests.post<User>('/account/register', user),
    switchOffice: (officeId: number) => requests.get<User>(`/account/switchoffice?newOfficeId=${officeId}`)
}

const AppUser = {
    list: () => requests.get<IAppUser[]>('/appuser'),
    create: (appUser: IAppUser) => requests.post<IAppUser>('/appuser', appUser),
    details: (id: string) => requests.get<IAppUser>(`/appuser/${id}`),
    update: (appUser: IAppUser) => requests.put<IAppUser>(`/appuser/${appUser.id}`, appUser),
    delete: (id: string) => requests.del<void>(`/appuser/${id}`),
    lookupUser: (email: string) => requests.get<LookupUser>(`/appuser/lookupusername?email=${email}`)
}

const agent = {
    Projects,
    Concrete, 
    Account,
    AppUser
}

export default agent;