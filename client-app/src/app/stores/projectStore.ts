import { makeAutoObservable } from "mobx";
import React from "react";
import agent from "../api/agent";
import { Project } from "../models/coreInterface";

export default class ProjectStore {
    projects: Project[] = [];
    selectedProject: Project | undefined = undefined;
    loadingInitial = false;
    display = false;
    search: string = "";

    constructor() {
        makeAutoObservable(this)
    }

    
    loadProjects = async () => {
        this.setLoadingInitial(true);
        let officeId = 1;
        try {
            const list = await agent.Projects.list(officeId);
            console.log(list);
            list.forEach(p => {
                //activity.date = activity.date.split('T')[0];
                //this.activities.push(activity);
                //this.activityRegistry.set(activity.id, activity);
                this.projects.push(p);
            });
            this.setLoadingInitial(false);
        } catch (error)
        {
            console.log(error);
            this.setLoadingInitial(false);
        }
    }

    selectProject = (id: number) => {
        this.selectedProject = this.projects.find(a => a.id === id);
        //this.selectedActivity = this.activityRegistry.get(id);
    }

    setLoadingInitial = (state:boolean) => {
        this.loadingInitial = state;
    }
    setDisplay = (state: boolean) => {
        this.display = state;
    }
    setSearch = (search: string) => {
        this.search = search;
    }
}

