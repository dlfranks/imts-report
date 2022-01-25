import React from "react";
import { Label, Table } from "semantic-ui-react";
import { LooseObject } from '../../app/stores/concreteStore';

interface Props{
    data: LooseObject[];
}

export default function DataTable({ data }: Props) {
    const headers = () => {
        

        let headerList: string[];
            let headerData: LooseObject = data[0];
            headerList = (Object.keys(headerData) as Array<keyof typeof headerData>).reduce((accumulator, current) => {
                accumulator.push(current);
                return accumulator;
            }, [] as (typeof headerData[keyof typeof headerData])[]);
        
            console.log(headerList)

            return headerList;

        

    }
    
    const createBody = () => {

        const body =  data.map((row) => {
            return (<Table.Row>
                {
                    row.map((column: any) => {
                        return (<Table.Cell>{column}</Table.Cell>);
                    })
                }
            </Table.Row>)
        });

        return body;
    }

    const createRow = () => {

    }

    const createColumn = () => {

    }

    return (
        <div className='container_table'>
        <Table celled>
            <Table.Header>
                    <Table.Row>
                    {
                          headers().map((header) => {
                              return (<Table.HeaderCell key={header}>{header}</Table.HeaderCell>);
                        })
                    }
                    
                </Table.Row>
                </Table.Header>
                
            <Table.Body>
                {
                    data.map((row) => {
                        return (<Table.Row>
                            {
                                row.map((column: any) => {
                                    return (<Table.Cell>{column}</Table.Cell>);
                                })
                            }
                        </Table.Row>)
                            })

                }
            </Table.Body>
            
            </Table>
            </div>
    )
}