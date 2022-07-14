import { makeAutoObservable, runInAction } from "mobx";
import agent from "../api/agent";
import { IAppUser, AppUser } from '../models/user';



export default class UserStore {
  users: IAppUser[] = [];
  userRegistry = new Map<string, IAppUser>();
  loadingInitial = false;
  selectedActivity: IAppUser | undefined = undefined;

  constructor() {
    makeAutoObservable(this);
  }

  setLoadingInitial = (state: boolean) => {
    this.loadingInitial = state;
  };
  private setAppUserRegistry = (appUser: IAppUser) => {
    this.userRegistry.set(appUser.id!, appUser);
  };
  private setAppUser = (appUser: IAppUser) => {
    this.users.push(appUser);
    this.setAppUserRegistry(appUser);
  };
  private getAppUser = (id: string) => {
    return this.userRegistry.get(id);
  };
  createAppUser = async (appUser: IAppUser) => {
    try {
      await agent.AppUser.create(appUser);
      const newAppUser = new AppUser(appUser);
      this.setAppUser(newAppUser);
    } catch (error) {
      console.log(error);
      return error;
    }
  };
  updateAppUser = async (appUser: IAppUser) => {
    try {
      await agent.AppUser.update(appUser);
      runInAction(() => {
        if (appUser.id)
        {
          let updatedAppUser = { ...this.getAppUser(appUser.id!), ...appUser}
          
          const users = this.users.map((user) =>
            user.id === appUser.id ? { ...user, ...updatedAppUser } : user
          );

          this.users = users;
        }
      });
      
      
    } catch (error) {
      console.log(error);
    }
  };
  loadAppUser = async (id: string) => {
    let appUser = this.getAppUser(id);
    if (appUser) {
      runInAction(() => {
        this.selectedActivity = appUser;
      });

      return appUser;
    } else {
      this.loadingInitial = true;
      try {
        appUser = await agent.AppUser.details(id);
        this.setAppUser(appUser);
        runInAction(() => {
          this.selectedActivity = appUser;
        });
        this.setLoadingInitial(false);
        return appUser;
      } catch (error) {
        console.log(error);
        this.setLoadingInitial(false);
      }
    }
  };

  loadAppUsers = async () => {
    this.setLoadingInitial(true);
    console.log(`ready to load users`);
    try {
      const result = await agent.AppUser.list();
      console.log(`finished to load users: ${result} `);
      runInAction(() => {
        this.users = [];
        this.userRegistry = new Map<string, IAppUser>();
        console.log(`this.users cleared `);
        result.forEach((appUser) => {
        this.setAppUserRegistry(appUser);
        this.setAppUser(appUser);
        console.log(`each user : ${appUser}`);
      });
      });
      
      console.log(`this.users completed`);
      
      this.setLoadingInitial(false);
    } catch (error) {
      console.log(error);
      this.setLoadingInitial(false);
    }
  };

  deleteAppUser = async (id: string) => {
    this.setLoadingInitial(true);
    try {
      await agent.AppUser.delete(this.selectedActivity?.id!);
      this.setLoadingInitial(false);
    } catch (error) {
      console.log(error);
      this.setLoadingInitial(false);
    }
  };

  lookupUsername = async (email: string) => {
    this.setLoadingInitial(true);
    try {
      let result = await agent.AppUser.lookupUser(email);
      this.setLoadingInitial(false);
      return result;
    } catch (error) {
      console.log(error);
      this.setLoadingInitial(false);
    }
  };
}
