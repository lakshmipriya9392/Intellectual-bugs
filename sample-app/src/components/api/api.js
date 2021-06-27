import React, { useState, useEffect } from 'react'
import axios from 'axios'


function api() {

    let [beta, getBeta] = useState()
    let num = 1

    useEffect(() => {
        async function alpha() {
            const res = await axios.get(`https://jsonplaceholder.typicode.com/posts/`)
            getBeta(res.data[6].body)

            //Don't directly put your result here
        }
        alpha()

    })

    let alpah = (e) => {
        console.log(e.ty)
    }

    return (


        <div className='text-center '>
            {/* {beta.map((val) => {
                return <div>{val.id}</div>
            })} */}
            {beta}
            <button className='border-4' onClick={alpah}>Click</button>
        </div>
    )
}

export default api








