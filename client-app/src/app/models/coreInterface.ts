export interface Project {
    id: number;
    number: string;
    name: string;
    officeId: number;
    
    
}

export interface Office {
    id: number;
    name: string;
    resLocaleId: number;
    unitSystme: number;
    region: number;
}