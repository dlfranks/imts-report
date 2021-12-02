import { createContext, useContext } from "react";
import ModalStore from './modalStore';
import CommonStore from './commonStore';
import ProjectStore from "./projectStore";
import ConcreteStore from "./concreteStore";


interface Store{
    modalStore: ModalStore;
    commonStore: CommonStore;
    projectStore: ProjectStore;
    concreteStore: ConcreteStore;
}

export const store: Store = {
    modalStore: new ModalStore(),
    commonStore: new CommonStore(),
    projectStore: new ProjectStore(),
    concreteStore: new ConcreteStore()
}

export const StoreContext = createContext(store);

export function useStore() {
    return useContext(StoreContext);
}