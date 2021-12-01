import { makeAutoObservable } from "mobx";
import React from "react";
import agent from "../api/agent";
import { ConcreteParam } from '../models/concreteInterface';

export default class FieldConcreteTest {
    loadingInitial = false;
    downloaded = false;

    constructor() {
        makeAutoObservable(this)
    }

    download = async (params: ConcreteParam) => {
        try {
            await agent.FieldConcreteTest.json(params)
        } catch (error)
        {
            throw error;
        }
    }

}