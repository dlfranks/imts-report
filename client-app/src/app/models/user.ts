import { IDValuePair } from './coreInterface';
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

export interface AppUser {
    id: string;
    firstName: string;
    lastName: string;
    userName: string;
    email: string;
    isImtsUser: boolean;
    imtsUserName?: string;
    imtsEmployeeId?: Number;
    password: string;
    roleName: string;
    mainOfficeId: number;
}

export interface AppUserColumns {
    [index: number]: { title: string, field: string };
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
