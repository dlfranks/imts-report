import React from 'react';
import { useField } from 'formik';
import { Form, Select, Label } from 'semantic-ui-react';

interface Props {
    placeholder: string;
    name: string;
    options: any;
    label?: string;
    onChange: () => void;
}

export default function MySelectInput(props: Props) {
    const [field, meta, helpers] = useField(props.name);
    return (
        <>
            <label>{props.label}</label>
            <div>
                <Select
                    style={{width:300}}
                    clearable
                    options={props.options}
                    value={field.value || null}
                    onChange={(e, d) => helpers.setValue(d.value)}
                    onBlur={() => helpers.setTouched(true)}
                    placeholder={props.placeholder}
                />
            </div>
            
            {meta.touched && meta.error ? (
                <Label basic color='red'>{ meta.error}</Label>
            ) : null}
        </>
    )
}