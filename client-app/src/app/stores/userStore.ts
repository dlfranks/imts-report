import { makeAutoObservable, runInAction } from 'mobx';
import React from 'react';
import agent from '../api/agent';
import { AppUser, User, UserFormValues } from '../models/user';
import { store } from './store';
import { history } from "../..";

export default class UserStore {
    user: User | null = null;
    users: AppUser[] = [];
    userRegistry = new Map<string, AppUser>();
    loadingInitial = false;


    constructor() {
        makeAutoObservable(this)
    }

    get isLoggedIn() {
        return !!this.user;
    }

    setLoadingInitial = (state: boolean) => {
        this.loadingInitial = state;
    }
    private setAppUserRegistry = (appUser: AppUser) => {
        this.userRegistry.set(appUser.id, appUser);
    }
    setActivity = (appUser: AppUser) => {
        this.users.push(appUser);
    }

    loadAppUsers = async () => {
        this.setLoadingInitial(true);
        try {
            const result = await agent.Administration.list();
            result.forEach(appUser => {
                this.setAppUserRegistry(appUser);
                this.setActivity(appUser);
            });
            this.setLoadingInitial(false);
            
        } catch (error) {
            console.log(error);
            this.setLoadingInitial(false);
        }
        
    }
    
    login = async (creds: UserFormValues) => {
        try {
            const user = await agent.Account.login(creds);
            store.commonStore.setToken(user.token);
            runInAction(() => this.user = user);
            history.push('/fieldData');
            store.modalStore.closeModal();
        } catch (error) {
            throw error;
        }
    }

    logout = () => {
        store.commonStore.setToken(null);
        window.localStorage.removeItem('jwt');
        this.user = null;
        history.push('/');
    }

    getUser = async () => {
        try {
            const user = await agent.Account.current();
            runInAction(() => this.user = user);
        } catch (error) {
            console.log(error);
        }
    }
    
    register = async(creds: UserFormValues) => {
        try {
            const user = await agent.Account.register(creds);
            store.commonStore.setToken(user.token);
            runInAction(() => this.user = user);
            history.push('/fieldData');
            store.modalStore.closeModal();
        } catch (error) {
            throw error;
        }
    }
}