// @ts-check
import { defineConfig } from 'astro/config';
import starlight from '@astrojs/starlight';
import mdx from '@astrojs/mdx';

// https://astro.build/config
export default defineConfig({
	site:'bindsight.rhomicro.com',
	integrations: [
		starlight({
			title: 'BindSight',
			social: [
				{
					icon: 'github',
					label: 'GitHub',
					href: 'https://github.com/sleepwellpupper/RhoMicro.BindSight'
				}
			],
			sidebar: [
				{
					label: 'Tutorials',
					autogenerate: { directory: 'reference' },
				},
				{
					label: 'Guides',
					autogenerate: { directory: 'guides' },
				},
				{
					label: 'Reference',
					autogenerate: { directory: 'reference' },
				},
			],
		}),
	],
});
