import { createContext, useContext } from "react";
import ModalStore from './modalStore';
import CommonStore from './commonStore';
import ProjectStore from "./projectStore";
import ConcreteStore from "./concreteStore";
import UserStore from "./userStore";


interface Store{
    modalStore: ModalStore;
    commonStore: CommonStore;
    projectStore: ProjectStore;
    concreteStore: ConcreteStore;
    userStore: UserStore;
}

export const store: Store = {
    modalStore: new ModalStore(),
    commonStore: new CommonStore(),
    projectStore: new ProjectStore(),
    concreteStore: new ConcreteStore(),
    userStore: new UserStore(),
}

export const StoreContext = createContext(store);

export function useStore() {
    return useContext(StoreContext);
}