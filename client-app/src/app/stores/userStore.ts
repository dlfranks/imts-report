import { makeAutoObservable, runInAction } from "mobx";
import agent from "../api/agent";
import { AppUser} from "../models/user";


export default class UserStore {
    
  users: AppUser[] = [];
  userRegistry = new Map<string, AppUser>();
  loadingInitial = false;

  constructor() {
    makeAutoObservable(this);
  }

  setLoadingInitial = (state: boolean) => {
    this.loadingInitial = state;
  };
  private setAppUserRegistry = (appUser: AppUser) => {
    this.userRegistry.set(appUser.id, appUser);
  };
  setAppUser = (appUser: AppUser) => {
    this.users.push(appUser);
  };

  loadAppUsers = async () => {
    this.setLoadingInitial(true);
    try {
        const result = await agent.Administration.list();
        runInAction(() => {
            this.users = [];
            this.userRegistry = new Map<string, AppUser>();
        });
        

      result.forEach((appUser) => {
        this.setAppUserRegistry(appUser);
        this.setAppUser(appUser);
      });
      this.setLoadingInitial(false);
    } catch (error) {
      console.log(error);
      this.setLoadingInitial(false);
    }
  };

  

  
}
