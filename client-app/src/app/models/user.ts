import { IDValuePair } from "./coreInterface";
import { ErrorMessage } from "formik";
export interface User {
  id: string;
  username: string;
  displayName: string;
  token: string;
  image?: string;
  officeId: number;
  memberOffices: IDValuePair[];
}

export interface UserFormValues {
  email: string;
  password: string;
  displayName?: string;
  username?: string;
}
export interface IAppUser {
  id?: string;
  firstName: string;
  lastName: string;
  userName: string;
  email: string;
  isImtsUser: boolean;
  imtsUserName?: string;
  imtsEmployeeId?: Number;
  password?: string;
  roleName: string;
}



export class AppUser implements IAppUser {
  id?: string = undefined;
  firstName: string = "";
  lastName: string = "";
  userName: string = "";
  email: string = "";
  isImtsUser: boolean = false;
  imtsUserName?: string = undefined;
  imtsEmployeeId?: Number;
  password?: string = "";
  roleName: string = "";

  constructor(appUser?: AppUser) {
    if (appUser) {
      this.id = appUser.id;
      this.firstName = appUser.firstName;
      this.lastName = appUser.lastName;
      this.userName = appUser.userName;
      this.email = appUser.email;
      this.isImtsUser = appUser.isImtsUser;
      this.imtsEmployeeId = appUser.imtsEmployeeId;
      this.imtsUserName = appUser.imtsUserName;
      this.password = appUser.password;
      this.roleName = appUser.roleName;
    }
  }
}

export interface AppUserColumns {
  [index: number]: { title: string; field: string };
}
export const AppUserColumnsList = [
  {
    title: "First Name",
    field: "firstName",
  },
  {
    title: "Last Name",
    field: "lastName",
  },
  {
    title: "User Name",
    field: "userName",
  },
  {
    title: "Email",
    field: "email",
  },
  {
    title: "Is IMTS user?",
    field: "isImtsUser",
  },
  {
    title: "IMTS User Name",
    field: "imtsUserName",
  },
  {
    title: "IMTS Employee ID",
    field: "imtsEmployeeId",
  },
  {
    title: "Password",
    field: "password",
  },
  {
    title: "Role",
    field: "roleName",
  },
];

export interface LookupUser {
  isValidToCreate: boolean;
  appUserDTO: IAppUser;
  errmsg: string;
  succmsg: string;
}
