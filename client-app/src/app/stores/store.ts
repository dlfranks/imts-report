import { createContext, useContext } from "react";
import ModalStore from './modalStore';
import CommonStore from './commonStore';
import ProjectStore from "./projectStore";


interface Store{
    modalStore: ModalStore;
    commonStore: CommonStore;
    projectStore: ProjectStore;
}

export const store: Store = {
    modalStore: new ModalStore(),
    commonStore: new CommonStore(),
    projectStore: new ProjectStore()
}

export const StoreContext = createContext(store);

export function useStore() {
    return useContext(StoreContext);
}