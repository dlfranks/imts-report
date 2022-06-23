import { makeAutoObservable, reaction, runInAction } from 'mobx';
import { ServerError } from '../models/serverError';
import { User, UserFormValues } from '../models/user';
import { store } from './store';
import { history } from "../..";
import agent from '../api/agent';


export default class CommonStore {
    error: ServerError | null = null;
    token: string | null = window.localStorage.getItem('jwt');
    appLoaded = false;
    user: User | null = null;

    constructor() {
        makeAutoObservable(this);
        reaction(
            () => this.token,
            token => {
                if (token) {
                    window.localStorage.setItem('jwt', token)
                } else {
                    window.localStorage.removeItem('jwt');
                }
            }
        )
    }

    get isLoggedIn() {
        return !!this.user;
    }
    
    login = async (creds: UserFormValues) => {
        try {
          const user = await agent.Account.login(creds);
          this.setToken(user.token);
          runInAction(() => (this.user = user));
          history.push("/administration");
          store.modalStore.closeModal();
        } catch (error) {
          throw error;
        }
      };
    
      logout = () => {
        this.setToken(null);
        window.localStorage.removeItem("jwt");
        this.user = null;
        history.push("/");
      };
    
      getUser = async () => {
        try {
            const user = await agent.Account.current();
            //this.user = user;
          runInAction(() => (this.user = user));
        } catch (error) {
            console.log(error);
            history.push('/');
        }
        };
    switchOffice = async (officeId: number) => {
        try {
            const user = await agent.Account.switchOffice(officeId);
          this.setToken(user.token);
          console.log(`switched office: ${user.officeId}`);
            runInAction(() => {
                this.user = { ...this.user, ...user };
                
            });
            //store.userStore.loadAppUsers();
            //history.push("/administration");
          return user;
            
        } catch (error) {
            console.log("Switch Office: " + error);
        }
    };
    
    setServerError = (error: ServerError) => {
        this.error = error;
    }

    setToken = (token: string | null) => {
        if (token) window.localStorage.setItem('jwt', token);
        this.token = token;
    }

    setAppLoaded = () => {
        this.appLoaded = true;
    }

    register = async (creds: UserFormValues) => {
    try {
      const user = await agent.Account.register(creds);
      store.commonStore.setToken(user.token);
      runInAction(() => (this.user = user));
      history.push("/fieldData");
      store.modalStore.closeModal();
    } catch (error) {
      throw error;
    }
  };
}