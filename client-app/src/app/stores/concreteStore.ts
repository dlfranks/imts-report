import { makeAutoObservable } from "mobx";
import React from "react";
import agent from "../api/agent";
import { ConcreteParam } from '../models/concreteInterface';

export default class ConcreteStore {
    loadingInitial = false;
    downloaded = false;

    constructor() {
        makeAutoObservable(this)
    }

    concreteApiUrl = 'http://localhost:5000/api/FieldConcreteTest';
    download = async (params: ConcreteParam) => {
        try {
            await agent.Concrete.json(params)
        } catch (error)
        {
            throw error;
        }
    }

}